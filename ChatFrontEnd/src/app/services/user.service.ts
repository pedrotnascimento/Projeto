import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { API_CONTROLER } from '../../environments/environment';
import { getHeaders } from './api-utils';



@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) { }

  create = (login: string, password: string) => {
    const body = {
      "name": login,
      "password": password,
      "email": login+"@gmail.com",
      "role": "client"
    };
    return this.http.post(`${API_CONTROLER}/api/user/create`, body, getHeaders());
  }

  login = (login:string , password: string) => {
    const body = {
      "userID": login,
      "password": password,
      "role": "client"
    };
    return this.http.post(`${API_CONTROLER}/api/user/login`, body, getHeaders());
  }

  getLoggedUser= () => {
    return this.http.get(`${API_CONTROLER}/api/user/logged`, getHeaders());
  }
}
