import { Observable, of } from "rxjs";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})

export class HttpService{
    constructor(){}

    public handleError<T>(operation = 'operation', result?: T){
        return (error: any) : Observable<T> =>{
            console.log(`${operation} failed: ${error.message}`);
            return of(result as T)
        }
    }
}