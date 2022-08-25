import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { ChatModel } from '../interfaces/chatInterface';
@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  public data: ChatModel[] = [];
  action: any = null;

  private hubConnectionStockBot: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5272/stocksocket')
    .build();

  private hubConnectionMessages: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5002/messagesocket')
    .build();

  public startConnection = () => {
    //this.hubConnection = new signalR.HubConnectionBuilder()
    //  .withUrl('ws://localhost:5272/stocksocket')
    //  .build();
    this.hubConnectionStockBot
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))

    this.hubConnectionMessages
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public addTransferStockCommandListener = (action: any) => {
    this.hubConnectionStockBot.on('stockCommandReceived', (data) => {
      this.data = data;
      if (action != null) {
        action(data);
      }
      console.log(data);
    });
  }

    public addTransferMessageListener = (action: any) => {
      this.hubConnectionMessages.on('messageReceived', (data) => {
        this.data = data;
        if (action != null) {
          action(data);
        }
      console.log(data);
      });
  }

  public addTransferMessageSender = (obj: any) => {
    this.hubConnectionMessages.invoke('MessageSent', obj)
      .catch(x => console.error(x));
  }
}
