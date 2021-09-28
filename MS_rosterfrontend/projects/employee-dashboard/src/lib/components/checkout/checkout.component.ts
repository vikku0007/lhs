import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
interface Gender {
  value: string;
  name: string;
}

// For Condition
interface Condition {
  value: string;
  name: string;
}

// For Symptom
interface Symptom {
  value: string;
  name: string;
}

// For Consume Alcohol
interface ConsumeAlcohol {
  value: string;
  name: string;
}

// For Document Type
interface DocumentType {
  value: string;
  name: string;
}

//For Progress Notes
export interface PeriodicElementProgressNotes {
  date: string;
  progressNote: string;
}

//For Education
const ELEMENT_DATA_PROGRESS_NOTES: PeriodicElementProgressNotes[] = [
  {date: 'Lorem Ipsum', progressNote: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.'},
  {date: 'Pretium Nunc', progressNote: 'Donec sit amet ligula posuere, malesuada turpis nec.'},
];

export interface RequiredDocumentElement {
  documentName: string;
  documentType: string;
  description: string;
  dateOfIssue: string;
  dateOfExpiry: string;
}

const REQUIRED_DOCUMENT_DATA: RequiredDocumentElement[] = [
  {documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020'},
  {documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020'},
  {documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020'},
  {documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020'},
  {documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020'},
];

export interface OtherDocumentElement {
  documentName: string;
  documentType: string;
  description: string;
  dateOfIssue: string;
  dateOfExpiry: string;
}

const OTHER_DOCUMENT_DATA: OtherDocumentElement[] = [
  {documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020'},
  {documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020'},
  {documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020'},
  {documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020'},
  {documentName: 'Document 1', documentType: 'Doc', description: 'Lorem Ipsum', dateOfIssue: '5-Mar-2020', dateOfExpiry: '5-Apr-2020'},
];


@Component({
  selector: 'lib-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  getErrorMessage:'Please Enter Value';

  //For Person
  genders: Gender[] = [
    {value: '', name: 'Select Gender'},
    {value: 'gender-1', name: 'Male'},
    {value: 'gender-2', name: 'Female'},
    {value: 'gender-3', name: 'Other'}
  ];
  selectedGender = 'gender-2';

  //For Condition
  conditions: Condition[] = [
    {value: '', name: 'Select Condition'},
    {value: 'condition-1', name: 'Asthma'},
    {value: 'condition-2', name: 'Cancer'},
    {value: 'condition-3', name: 'Cardiac Disease'}
  ];
  selectedCondition = 'condition-1';

  //For Symptom
  symptoms: Symptom[] = [
    {value: '', name: 'Select Symptom'},
    {value: 'symptom-1', name: 'Cardiovascular'},
    {value: 'symptom-2', name: 'Hematological'},
    {value: 'symptom-3', name: 'Lymphatic'}
  ];
  selectedSymptom = 'symptom-1';

  //For ConsumeAlcohol
  consumeAlcohols: ConsumeAlcohol[] = [
    {value: '', name: 'Select An Option'},
    {value: 'consumeAlcohol-1', name: 'Occasionally'},
    {value: 'consumeAlcohol-2', name: 'Yes'},
    {value: 'consumeAlcohol-3', name: 'No'}
  ];
  selectedConsumeAlcohol = 'consumeAlcohol-1';

  //For Progress Notes
  displayedColumnsProgressNotes: string[] = ['date', 'progressNote'];
  dataSourceProgressNotes = new MatTableDataSource(ELEMENT_DATA_PROGRESS_NOTES);

  //For Person
  dtypes: DocumentType[] = [
    {value: '', name: 'Select Document Type'},
    {value: 'dtype-1', name: 'Doc'},
    {value: 'dtype-2', name: 'Excel'},
    {value: 'dtype-3', name: 'Powerpoint'}
  ];
  selectedDocumentType = 'dtype-1';

  displayedColumnsRequired: string[] = ['documentName', 'documentType', 'description', 'dateOfIssue', 'hasExpiry', 'dateOfExpiry', 'alert', 'action'];
  dataSourceRequired = new MatTableDataSource(REQUIRED_DOCUMENT_DATA);

  displayedColumnsOther: string[] = ['documentName', 'documentType', 'description', 'dateOfIssue', 'hasExpiry', 'dateOfExpiry', 'alert', 'action'];
  dataSourceOther = new MatTableDataSource(OTHER_DOCUMENT_DATA);

  delete(elm) {
    this.dataSourceRequired.data = this.dataSourceRequired.data.filter(i => i !== elm)
    this.dataSourceOther.data = this.dataSourceOther.data.filter(i => i !== elm)
  }

  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor() { }

  ngOnInit(): void {
    this.dataSourceProgressNotes.sort = this.sort;
    this.dataSourceRequired.sort = this.sort;
    this.dataSourceOther.sort = this.sort;
  }

}
