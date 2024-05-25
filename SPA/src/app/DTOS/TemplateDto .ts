import { TemplateType } from "./TemplateType ";

export interface TemplateDto {
  id: number;
  categoryId: number;
  name: string;
  templateType: TemplateType;
  templateText: string;
}

