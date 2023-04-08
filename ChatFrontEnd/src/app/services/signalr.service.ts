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
    this.hubConnectionStockBot
      .start();

    this.hubConnectionMessages
      .start();
  }

  public addTransferStockCommandListener = (action: any) => {
    this.hubConnectionStockBot.on('stockCommandReceived', (data) => {
      this.data = data;
      if (action != null) {
        action(data);
      }
    });
  }

    public addTransferMessageListener = (action: any) => {
      this.hubConnectionMessages.on('messageReceived', (data) => {
        this.data = data;
        if (action != null) {
          action(data);
        }
      
      });
  }

  public addTransferMessageSender = (obj: any) => {
    this.hubConnectionMessages.invoke('MessageSent', obj)
      .catch(x => console.error(x));
  }
}
