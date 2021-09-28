import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpEvent, HttpHandler, HttpRequest, HttpResponse, HttpErrorResponse, HttpInterceptor } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { LoaderService } from 'src/app/domain/services/loader/loader.service';


@Injectable(
  // providedIn: 'root'
)
export class InterceptorService implements HttpInterceptor {

  constructor(private loaderService: LoaderService) { }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler

  ): Observable<HttpEvent<any>> {
    this.loaderService.start();
    if (req.body) {

      req.headers.append('Content-Type', 'application/json');
      // req.headers.append('If-Modified-Since', 'Mon, 26 Jul 1997 05:00:00 GMT');
      // req.headers.append('Cache-Control', 'no-cache');
      // req.headers.append('Pragma', 'no-cache');
      // req.headers.append('Server', 'Microsoft-IIS/10.0');
      // req.headers.append('X-Powered-By', 'ASP.NET');
      // req.headers.append('Date', Date.now.toString());      
    }
    const token = localStorage.getItem('access_token') ? localStorage.getItem('access_token') : null;
    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: 'Bearer ' + token
        }
      });
    }

    return next.handle(req).pipe(
      tap(
        (event: HttpEvent<any>) => {
          if (event instanceof HttpResponse) {
            this.loaderService.stop();
          }
        },
        (err: any) => {
          if (err instanceof HttpErrorResponse) {
            this.loaderService.stop();
          }
        },
        () => {
          // this.loaderService.stop();
        }

      )
    )
  }
}
