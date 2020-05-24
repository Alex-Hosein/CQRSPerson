import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Person } from "../models/person";
import { HttpService } from "./http.service";
import { catchError } from "rxjs/operators";

@Injectable({
    providedIn: 'root'
})

export class PersonService{
    constructor(private _http: HttpClient, private _httpService: HttpService){      
    }

    getPersons() : Observable<any>{
        return this._http.get<any>(`${environment.ApiUrl}`)
            .pipe(catchError(this._httpService.handleError<any>("Error getting person")));
    }

    addPersons(person: Person) : Observable<any>{
        return this._http.post<any>(`${environment.ApiUrl}`,person, {});
    }
}