import { Route } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';

export const routes: Route[] = [
  {
    path: '',
    redirectTo: RoutesPath.HOME,
    pathMatch: 'full'
  },
  {
    path: RoutesPath.AUTHENTICATION,
    redirectTo: `${RoutesPath.AUTHENTICATION}/${RoutesPath.LOGIN}`,
  },

  {
    path: RoutesPath.HOME,
    loadComponent: () => import('./components/home/home.component').then(m => m.HomeComponent),
    children: [
      // {
      //   path: RoutesPath.OFFER,
      //   component: OfferComponent,
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
    ]
  },
  {
    path: RoutesPath.ADD_OFFER,
    loadComponent: () => import('./components/add-offer/add-offer.component').then(m => m.AddOfferComponent),
  },
  {
    path: RoutesPath.AUTHENTICATION,
    loadComponent: () => import('./components/authentication/authentication.component').then(m => m.AuthenticationComponent),
    children: [
      {
        path: RoutesPath.LOGIN,
        loadComponent: () => import('./components/authentication/components/login/login.component').then(m => m.LoginComponent),
      },
      {
        path: RoutesPath.REGISTER,
        loadComponent: () => import('./components/authentication/components/register/register.component').then(m => m.RegisterComponent),
      },
    ]
  },

  {
    path: '**',
    redirectTo: RoutesPath.HOME,
  },
];