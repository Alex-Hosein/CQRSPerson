export class Person{
    personId : number;
    firstName : string;
    lastName : string;
    age : string;
    interests : string;
    image : string;

    constructor(json : any){
        if(json){
            this.personId = json.personId;
            this.firstName = json.firstName;
            this.lastName = json.lastName;
            this.age = json.age;
            this.interests = json.interests;
            this.image = json.image;
        }
    }
}