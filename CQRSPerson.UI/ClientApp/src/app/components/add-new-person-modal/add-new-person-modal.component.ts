import { Component, Output, EventEmitter } from "@angular/core";
import { FormGroup, FormBuilder } from "@angular/forms";
import { MatDialogRef, MatDialog } from "@angular/material";
import { PersonService } from "src/app/services/person.service";
import { Person } from "src/app/models/person";

@Component({
    selector: 'add-new-person-modal',
    templateUrl: 'add-new-person-modal.component.html'
    // ,
    // styleUrls: ['add-new-person-modal.component.scss']
})

export class AddNewPersonModalComponent {
    addPersonForm: FormGroup;
    
    constructor(public dialogRef: MatDialogRef<AddNewPersonModalComponent>, private formBuilder: FormBuilder, private personService : PersonService)
    {
        this.addPersonForm = this.formBuilder.group({
            firstName : '',
            lastName : '',
            age : 0,
            interests : '',
            image: ''
        })
    }

    cancel(){
        this.dialogRef.close(false);
    }

    async onSubmit(formData){
        await this.personService.addPersons( new Person({
            "firstName": formData.firstName,
            "lastName" : formData.lastName,
            "age" : formData.age,
            "interests" : formData.interests,
            "image" : formData.image
        })).subscribe(res => {
            this.dialogRef.close(res);
        },
        err =>{
            this.dialogRef.close(err.error);
        })
    }
}