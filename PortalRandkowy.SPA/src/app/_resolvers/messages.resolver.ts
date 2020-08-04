import { Injectable } from '@angular/core';
import { Resolve, Router , ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MessagesResolver implements Resolve<Message[]>{

  pageNumber = 1;
  pageSize = 12;
  messageContainer = 'Nie przeczytane';

  constructor(private userService: UserService,
              private router: Router ,
              private alerify: AlertifyService,
              private authService: AuthService) {}

  resolve(route: ActivatedRouteSnapshot): Message[] | Observable<Message[]> | Promise<Message[]> {
    return this.userService.getMessages(this.authService.decodedToken.nameid, this.pageNumber, this.pageSize, this.messageContainer).pipe(
      catchError(error => {
        this.alerify.error('Problem z wyszukaniem wiadomosci');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
