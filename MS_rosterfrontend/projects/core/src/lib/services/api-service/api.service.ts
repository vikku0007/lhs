import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHandler } from '@angular/common/http';
import { ErrorHandlerService } from '../error-handler/error-handler.service';
import { CORE_CONFIG, CoreConfig } from '../../config/core-config';
import { catchError } from 'rxjs/operators'
import { throwError as observableThrowError, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService extends HttpClient {

  constructor(private httpHandler: HttpHandler, private errorHanlder: ErrorHandlerService,
    @Inject(CORE_CONFIG) private config: CoreConfig) {
    super(httpHandler);
  }

  get<T>(url: string, options?: {}): Observable<T> {
    return super.get<T>(`${this.config.baseUrl + url}`).pipe(catchError(this.handleError));
  }

  post<T>(url: string, params: {}, options?: {}): Observable<T> {
    return super.post<T>(`${this.config.baseUrl + url}`, params).pipe(catchError(this.handleError));
  }

  put<T>(url: string, params: {}, options?: {}): Observable<T> {
    return super.put<T>(`${this.config.baseUrl + url}`, params).pipe(catchError(this.handleError));
  }

  delete<T>(url: string, options?: {}): Observable<T> {
    return super.delete<T>(`${this.config.baseUrl + url}`).pipe(catchError(this.handleError));
  }

  public handleError = (error: Response) => {
    this.errorHanlder.handleError(error.status);
   return observableThrowError(error);
  }
}
