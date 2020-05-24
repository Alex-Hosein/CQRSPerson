import { Component, Inject, ViewChild, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { DataGridComponent } from '../datagrid/datagrid.component';
import { PersonService } from 'src/app/services/person.service';
import { Person } from 'src/app/models/person';
import { MatDialog } from '@angular/material';
import { AddNewPersonModalComponent } from '../add-new-person-modal/add-new-person-modal.component';
import { ToastrService } from 'ngx-toastr';
import { delay } from 'rxjs/operators';
import { timer } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  persons : Person[]
  search : string = '';
  isLoading: boolean = false;
  @ViewChild(DataGridComponent, {static:false}) private dataGridComponent: DataGridComponent;

  constructor(private personService:PersonService, private dialog:MatDialog, private toastr: ToastrService){
  }

  openDialog():void{
    const dialogRef = this.dialog.open(AddNewPersonModalComponent,{
      width:'50%'
    })
    dialogRef.afterClosed().subscribe(result => {
      if(result.content && result.content.personId > 0){
        this.toastr.success(result.informationalMessage)
        this.getPersons();
      }
      else if(result.errors){
        this.toastr.error(result.informationalMessage)
      }
    })
  }

  async onChange(e){
    if(this.isLoading){
      this.dataGridComponent.dataGridInstance.beginCustomLoading("Loading");
      timer(7000).subscribe(x => {
        this.dataGridComponent.dataGridInstance.endCustomLoading()
        this.filterGridResults();
      })   
    }
    else{
      this.filterGridResults();
    }
  }

  onChecked(e){
    this.isLoading = e.currentTarget.checked;
  }

  filterGridResults(){
    this.dataGridComponent.Persons = this.persons.filter(x => (x.firstName + x.lastName).toLowerCase().includes(this.search.toLowerCase()));
  }

  async ngOnInit() {
    await this.getPersons();
  }

  async getPersons(){
    await this.personService.getPersons()
      .subscribe(response => {
        this.persons = response.content;
        this.dataGridComponent.Persons = this.persons;
      });
  }
}

