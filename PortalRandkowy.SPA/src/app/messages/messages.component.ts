import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination, PaginationResult } from '../_models/pagination';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages: any;

  pagination: Pagination;
  messageContainer = 'Nie przeczytane';
  flagaOutbox = false;

  constructor(private userService: UserService,
              private authService: AuthService,
              private route: ActivatedRoute,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.messages = data.messages.result;
      this.pagination = data.messages.pagination;
    });
  }

  loadMessages() {
    this.userService.getMessages(this.authService.decodedToken.nameid , this.pagination.currentPage, this.pagination.itemsPerPage, this.messageContainer)
    .subscribe( res  => {
        this.messages = res.result;
        this.pagination = res.pagination;

        if (this.messages[0].messageContainer === 'Outbox') {
          this.flagaOutbox = true;
        } else {
          this.flagaOutbox = false;
        }
      }, error => {
        this.alertify.error(error);
      });
  }


  deleteMessage(id: number) {
    this.alertify.confirm('Czy napewno chcesz to usunac?', () => {
      this.userService.deleteMessage(id, this.authService.decodedToken.nameid).subscribe( () => {
        this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
        this.alertify.success(' udalo sie usunac');
      }, error => {
        this.alertify.error('chujowo');
      })
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }
}
