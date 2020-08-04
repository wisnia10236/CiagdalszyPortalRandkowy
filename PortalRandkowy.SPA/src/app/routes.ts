import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { UsereListComponent } from './users/usere-list/usere-list.component';
import { LikesComponent } from './likes/likes.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { UserDetailComponent } from './users/user-detail/user-detail.component';
import { UserDetailResolver } from './_resolvers/user-detail.resolver';
import { UserListResolver } from './_resolvers/user-list.resolver';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { LikesResolver } from './_resolvers/likes.resolver';
import { resolve } from 'url';
import { MessagesResolver } from './_resolvers/messages.resolver';



export const appRoutes: Routes = [
  // dodajemy do tablicy routingu okreslone przyporzadkowanie np strony z polubieniami ma pobierac component likescomponent
  { path: '', component: HomeComponent },

  {
    path: '', // zabezpieczenie , robimy jako podrzedne strony (children)
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [

      { path: 'uzytkownicy', component: UsereListComponent, resolve: {users: UserListResolver }},
      { path: 'uzytkownicy/:id', component: UserDetailComponent, resolve: {user: UserDetailResolver}},
      { path: 'uzytkownik/edycja', component: UserEditComponent, resolve: {user: UserEditResolver}, canDeactivate: [PreventUnsavedChanges]},
      { path: 'polubienia', component: LikesComponent, resolve: {users: LikesResolver}},
      { path: 'wiadomosci', component: MessagesComponent , resolve: { messages: MessagesResolver}},
    ],
  },
  { path: '**', redirectTo: '', pathMatch: 'full' },
];
