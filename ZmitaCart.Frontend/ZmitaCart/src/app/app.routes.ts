import { Route } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { isUserLoggedInGuard, isUserNotLoggedInGuard } from '@core/guards/auth.guard';

export const routes: Route[] = [
  {
    path: '',
    redirectTo: `${RoutesPath.HOME}/${RoutesPath.OFFERS}`,
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
      {
        path: RoutesPath.ACCOUNT,
        canActivate: [isUserLoggedInGuard],
        loadComponent: () => import('./components/account/account.component').then(m => m.AccountComponent),
      },
      {
        path: RoutesPath.OFFERS,
        loadComponent: () => import('./components/offers-main/offers-main.component').then(m => m.OffersMainComponent),
      },
      {
        path: RoutesPath.OFFERS_FILTERED,
        loadComponent: () => import('./components/offers-filtered/offers-filtered.component').then(m => m.OffersFilteredComponent),
      },
      // {
      // path: RoutesPath.OFFER, // żeby id oferty było też w url
      // loadComponent: () => import('./components/offers-main/offers-main.component').then(m => m.OffersMainComponent),
      // },
    ]
  },
  {
    path: RoutesPath.ADD_OFFER,
    canActivate: [isUserLoggedInGuard],
    loadComponent: () => import('./components/add-offer/add-offer.component').then(m => m.AddOfferComponent),
  },
  {
    path: RoutesPath.AUTHENTICATION,
    canActivate: [isUserNotLoggedInGuard],
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
    redirectTo: `${RoutesPath.HOME}/${RoutesPath.OFFERS}`,
  },
];