import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from '@app/app.routes';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes) ]
};


// {
//   providers: [
//     // importProvidersFrom(BrowserModule),
//     // importProvidersFrom(CommonModule),
//     importProvidersFrom(HttpClientModule),
//     importProvidersFrom(BrowserAnimationsModule),
//     importProvidersFrom(InputMaskModule),
//     importProvidersFrom(RouterModule.forRoot(APP_ROUTES)),
//     // provideHttpClient(withInterceptorsFromDi()),
//     provideAnimations()
//   ]
// }
//czy cos z tego trzeba dodawac?