export interface IExternalId{
    context : string,
    id :string
}

export class ExternalId implements IExternalId{
    context:string;
    id:string
}