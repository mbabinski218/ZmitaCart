import { Route } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { AddOfferComponent } from '@modules/add-offer/add-offer.component';
import { HomeComponent } from '@modules/home/home.component';

export const routes: Route[] = [
  {
    path: '',
    redirectTo: RoutesPath.HOME,
    pathMatch: 'full'
  },

  {
    path: RoutesPath.HOME,
    component: HomeComponent,
  },
  // {
  //   path: RoutesPath.OFFERS,
  //   component: OffersComponent,
  // },
  // {
  //   path: RoutesPath.OFFER,
  //   component: OfferComponent,
  // },
  {
    path: RoutesPath.ADD_OFFER,
    component: AddOfferComponent,
  },
  // {
  //   path: RoutesPath.AUTHENTICATION,
  //   component: AuthenticationComponent,
  // },
  // {
  //   path: RoutesPath.FAVOURITES,
  //   component: FavouritesComponent,
  // },
  // {
  //   path: RoutesPath.ACCOUNT,
  //   component: AccountComponent,
  // },
  // {
  //   path: RoutesPath.MESSAGES,
  //   component: MessagesComponent,
  // },
  {
    path: '**',
    redirectTo: RoutesPath.HOME,//TODO czy robić 404 not found page?
  },
  //TODO czy robić lazy load na komponenty?
];