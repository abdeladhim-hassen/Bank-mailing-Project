import { Component } from '@angular/core';
import { ServiceResponse } from '../../../DTOS/ServiceResponse';
import { TemplateDto } from '../../../DTOS/TemplateDto ';
import { EditTemplateComponent } from '../edit-template/edit-template.component';
import { TemplateType } from '../../../DTOS/TemplateType ';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TemplateService } from '../../../Services/template.service';
import { ModalService } from '../../../Services/modal.service';
import { SuccessToastComponent } from '../../../toast/success-toast/success-toast.component';
import { ErrorToastComponent } from '../../../toast/error-toast/error-toast.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-template-management',
  templateUrl: './template-management.component.html',
  styleUrl: './template-management.component.css'
})
export class TemplateManagementComponent {
  constructor(
    private templateService: TemplateService,
    private router: Router,
    private route: ActivatedRoute,
    private modalService: ModalService,
    private toastr: ToastrService
  ) {}

  TemplateType = TemplateType;
  models: TemplateDto[] = [];
  category = 1;
  show: boolean = false;

  ngOnInit(): void {
    this.route.queryParams.subscribe((params: Params) => {
      const templateType: TemplateType = params['TemplateType'];
      this.category = params['category'];
      if (templateType && this.category) {
        this.show = true;
        this.templateService.getAllTemplates(this.category, templateType).subscribe({
          next: (response: ServiceResponse<TemplateDto[]>) => {
            this.models = response.data || [];
          },
          error: (error) => {
            console.error(error.message);
          }
        });
      } else {
        this.show = false;
        this.models = [];
      }
    });
  }

  openModal(templateType: TemplateType, $event?: Event) {
    if ($event) {
      $event.preventDefault();
    }
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { category: this.category, TemplateType: templateType }
    });
  }

  openEditModal(model: TemplateDto) {
    this.modalService.openModal(EditTemplateComponent, { model: model });
  }

  onModalSubmit(updatedModel: TemplateDto) {
    this.templateService.updateTemplate(updatedModel).subscribe({
      next: (response: ServiceResponse<number>) => {
        this.toastr.success(response.message, 'Success', {
          toastComponent: SuccessToastComponent
        });
      },
      error: (error) => {
        this.toastr.error(error.message, 'Error', {
          toastComponent: ErrorToastComponent
        })
      }
    });
  }
}
