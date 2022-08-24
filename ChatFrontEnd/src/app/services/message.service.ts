import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { API_CONTROLER } from '../../environments/environment';
import { ChatModel } from '../interfaces/chatInterface';



@Injectable({
  providedIn: 'root'
})
export class MessageService {
  constructor(private http: HttpClient) { }

  create = (chatName: string) => {
    const body = { name: chatName };
    return this.http.post(`${API_CONTROLER}/api/message/create`, body);
  }

  list = () => {
    return this.http.get(`${API_CONTROLER}/api/message/list`);
  }
}
