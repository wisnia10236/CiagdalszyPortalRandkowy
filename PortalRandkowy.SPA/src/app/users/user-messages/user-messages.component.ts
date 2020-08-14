import { AuthService } from './../../_services/auth.service';
import { Component, OnInit, Input } from '@angular/core';
import { Message } from 'src/app/_models/message';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-user-messages',
  templateUrl: './user-messages.component.html',
  styleUrls: ['./user-messages.component.css']
})
export class UserMessagesComponent implements OnInit {

  @Input() recipientId: number;
  messages: any;
  newMessage: any = {};

  constructor(private userService: UserService,
              private authService: AuthService,
              private alerify: AlertifyService) { }

  ngOnInit() {
    this.loadMessages();
  }

  loadMessages() {
    let currentUserId = this.authService.decodedToken.nameid;
    currentUserId = parseInt(currentUserId);
    this.userService.getMessageThread(this.authService.decodedToken.nameid, this.recipientId).pipe(tap((mess: any) => {

      for (let i = 0 ; i < mess.length; i++) {

        if(mess[i].isRead === false && mess[i].recipientId === currentUserId) {
          this.userService.markAsRead(currentUserId, mess[i].id);
        }
      }
      console.log(mess);

    })

    )
        .subscribe(messages => {
        this.messages = messages;
    }, error => {
      this.alerify.error(error);
    });
  }

  sendMessage() {
    this.newMessage.recipientId = this.recipientId;
    this.userService.sendMessage(this.authService.decodedToken.nameid, this.newMessage).subscribe((message: Message) => {
      this.messages.unshift(message);
      this.newMessage.content = '';
    }, error => {
      this.alerify.error(error);
    });
  }
}
