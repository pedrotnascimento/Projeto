import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ChatRoomService } from './services/chatroom.service';
import { MessageService } from './services/message.service';
import { SignalrService } from './services/signalr.service';
import { StockBotService } from './services/stockBot.service';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ChatFrontEnd';
  chatName = "";
  userNameLogin = "";
  userNameRegister = "";
  passwordLogin = "";
  passwordRegister = "";
  isAuth: any = false;
  chatRooms: any[] = [];
  messages: any[] = [];
  chatRoomCurrent: any;
  messageField: string = "";
  userLogged: any;

  constructor(
    public signalRService: SignalrService,
    private http: HttpClient,
    private chatRoomService: ChatRoomService,
    private messageService: MessageService,
    private userService: UserService,
    private stockBotService: StockBotService,
  ) { }
  ngOnInit() {
    this.isAuth = !!localStorage.getItem("accessToken");
    const user = localStorage.getItem("user");
    if (!!user) {
      this.userLogged = JSON.parse(user);
    }

    this.signalRService.startConnection();
    this.signalRService.addTransferStockCommandListener(this.actionForStockSocket);
    this.signalRService.addTransferMessageListener(this.actionForMessageSocket);
    
    this.startHttpRequest();
    if (this.isAuth) {

      this.listChatRooms();
    }
  }

  login = () => {
    this.userService.login(this.userNameLogin, this.passwordLogin).subscribe((response: any) => {
      console.log("response login", response);
      const isAuth = response.authenticated;
      this.isAuth = isAuth;
      if (isAuth) {
        const accessToken = response.accessToken;
        localStorage.setItem("accessToken", accessToken);
        this.userService.getLoggedUser().subscribe(user => {
          this.userLogged = user;
          localStorage.setItem("user", JSON.stringify(user));
        })
        this.listChatRooms();
      }
    });
  }

  register = () => {
    this.userService.create(this.userNameRegister, this.passwordRegister).subscribe((response: any) => {
      console.log("response create user", response);

    });
  }

  createChat = () => {
    this.chatRoomService.create(this.chatName).subscribe(x => {
      console.log("created chatroom", x);
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
        console.log("response bot", x);
      });
      this.messageField = "";
      return;
    }
    const obj = {
      payload: this.messageField,
      chatRoomId: this.chatRoomCurrent.id,
      userId: this.userLogged.id,
      timestamp: new Date()
    };

    this.signalRService.addTransferMessageSender(obj);
    ///SENDING BY RESTFUL API
    //this.messageService.create(obj).subscribe(x => {
    //  console.log("message created rest", x);
    //  this.listMessages(this.chatRoomCurrent);

    //});
    this.messageField = "";
  }

  private listMessages(chat: any) {
    this.messageService.list(chat.id).subscribe((messages: any) => {
      console.log("messages rest", messages);
      this.messages = messages;
      this.messages = this.messages.reverse();
    });
  }

  private listChatRooms() {
    this.chatRoomService.list().subscribe((chats: any) => {
      this.chatRooms = chats;
    });
  }

  private startHttpRequest = () => {
    this.http.get('http://localhost:5272/stocksocket')
      .subscribe(res => {
        console.log(res);
      })
  }



  actionForStockSocket = (data: any) => {
    console.log("data from socket", data);
    const message = { payload: data, user: { userName: "Stock Bot" } };
    this.messages.push(message);
  }

  actionForMessageSocket = (data: any) => {
    console.log("messagefrom socket", data);
    const message = data;
    this.messages.push(message);
  }

}
