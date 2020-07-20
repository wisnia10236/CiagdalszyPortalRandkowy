import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginationResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-usere-list',
  templateUrl: './usere-list.component.html',
  styleUrls: ['./usere-list.component.css'],
})
export class UsereListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  genderList: any[] = [{value: 'mezczyzna', display: 'Mezczyzni'},
              {value: 'kobieta', display: 'Kobiety'}];
  zodiacSignList: any[] = [ {value: 'wszystkie', display: 'wszystkie'},
                  {value: 'Baran', display: 'baran'},
                  {value: 'Byk', display: 'byk'},
                  {value: 'Bliźnięta', display: 'bliznieta'},
                  {value: 'Rak', display: 'rak'},
                  {value: 'Lew', display: 'lew'},
                  {value: 'Panna', display: 'panna'},
                  {value: 'Waga', display: 'waga'},
                  {value: 'Skorpion', display: 'skorpion'},
                  {value: 'Strzelec', display: 'strzelec'},
                  {value: 'Koziorożec', display: 'koziorozec'},
                  {value: 'Wodnik', display: 'wodnik'},
                  {value: 'Ryby', display: 'ryby'}];
  userParams: any = {};
  pagination: Pagination;

  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data.users.result;
      this.pagination = data.users.pagination;
    });
    this.userParams.gender = this.user.gender === 'kobieta' ? 'mezczyzna' : 'kobieta';
    this.userParams.zodiacSign = 'wszystkie';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 100;
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  resetFilters() {
    this.userParams.gender = this.user.gender === 'kobieta' ? 'mezczyzna' : 'kobieta';
    this.userParams.zodiacSign = 'wszystkie';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 100;
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe((res: PaginationResult<User[]>) => {
        this.users = res.result;
        this.pagination = res.pagination;
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }
}
