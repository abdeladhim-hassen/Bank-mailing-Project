export interface EvenementDto {
  id: number;
  categoryId: number;
  canal: string;
  heureEnvoi: Date;
  type: string;
  nomClient: string;
  prenomClient: string;
}
