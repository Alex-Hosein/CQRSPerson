import { Component } from '@angular/core';
import DataGrid from 'devextreme/ui/data_grid';
import { Person } from 'src/app/models/person';

@Component({
    selector: 'app-datagrid',
    templateUrl: 'datagrid.component.html',
    styleUrls: ['datagrid.component.scss']
})
export class DataGridComponent {
    Persons: Person[] = [];
    dataGridInstance: DataGrid = null;

    constructor() {
    }
    saveGridInstance(e){
        this.dataGridInstance = e.component;
    }
}
