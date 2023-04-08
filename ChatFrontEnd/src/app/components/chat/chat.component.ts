import { HttpClient } from "@angular/common/http";
import { Component, Input, OnChanges, OnInit, SimpleChanges } from "@angular/core";
import { ChatRoomService } from "src/app/services/chatroom.service";
import { MessageService } from "src/app/services/message.service";
import { SignalrService } from "src/app/services/signalr.service";
import { StockBotService } from "src/app/services/stockBot.service";

@Component({
    selector: 'chat',
    templateUrl: './chat.component.html'
})
export class ChatComponent implements OnInit, OnChanges {
    @Input() isAuth = false;
    @Input() userLoggedId = undefined;
    chatRooms: any[] = [];
    messages: any[] = [];
    chatRoomCurrent: any;
    messageField: string = "";

    chatName = "";

    constructor(
        private http: HttpClient,
        private chatRoomService: ChatRoomService,
        private messageService: MessageService,
        private stockBotService: StockBotService,
        public signalRService: SignalrService,
    ) { }
    ngOnChanges(changes: SimpleChanges): void {
        if (this.isAuth) {
            this.listChatRooms();
        }
    }
    ngOnInit(): void {
        this.startSocket();
        if (this.isAuth) {
            this.listChatRooms();
        }
    }

    createChat = () => {
        this.chatRoomService.create(this.chatName).subscribe(x => {
            this.listChatRooms();
        });
    }

    enterChat = (chat: any) => {
        this.chatRoomCurrent = chat;
        this.listMessages(chat);
    }

    sendMessage = () => {

        if (this.messageField.includes("/stock=")) {
            const stock = this.messageField.split("=")[1];
            this.stockBotService.sendCommand(stock).subscribe((x: any) => {

            });
            this.messageField = "";
            return;
        }
        const obj = {
            payload: this.messageField,
            chatRoomId: this.chatRoomCurrent.id,
            userId: this.userLoggedId,
            timestamp: new Date()
        };

        this.signalRService.addTransferMessageSender(obj);
        this.messageField = "";
    }

    private startSocket() {
        this.signalRService.startConnection();
        this.signalRService.addTransferStockCommandListener(this.actionForStockSocket);
        this.signalRService.addTransferMessageListener(this.actionForMessageSocket);

        this.startHandShake();
    }

    private listMessages(chat: any) {
        this.messageService.list(chat.id).subscribe((messages: any) => {
            this.messages = messages;
            this.messages = this.messages.reverse();
        });
    }

    private listChatRooms() {
        this.chatRoomService.list().subscribe((chats: any) => {
            this.chatRooms = chats;
        });
    }

    private startHandShake = () => {
        this.http.get('http://localhost:5272/stocksocket')
            .subscribe(() => { });
    }

    actionForStockSocket = (data: any) => {
        const message = { payload: data, user: { userName: "Stock Bot" } };
        this.messages.push(message);
    }

    actionForMessageSocket = (data: any) => {
        const message = data;
        this.messages.push(message);
    }

}