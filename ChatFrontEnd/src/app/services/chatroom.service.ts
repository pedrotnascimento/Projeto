import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { API_CONTROLER } from '../../environments/environment';
import { ChatModel } from '../interfaces/chatInterface';
import { getHeaders } from './api-utils';



@Injectable({
  providedIn: 'root'
})
export class ChatRoomService {
  constructor(private http: HttpClient) { }

  create = (chatName: string) => {
    

    const body = { name: chatName };
    return this.http.post(`${API_CONTROLER}/api/chatroom/create`, body, getHeaders());
  }

  list = () => {
    return this.http.get(`${API_CONTROLER}/api/chatroom/list`, getHeaders());
  }
}
