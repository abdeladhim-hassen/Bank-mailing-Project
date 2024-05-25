import { Component, EventEmitter, Output } from '@angular/core';
import { TemplateDto } from '../../../DTOS/TemplateDto ';
import { Subscription } from 'rxjs';
import { ModalService } from '../../../Services/modal.service';
import { TemplateService } from '../../../Services/template.service';
import { ServiceResponse } from '../../../DTOS/ServiceResponse';

@Component({
  selector: 'app-edit-template',
  templateUrl: './edit-template.component.html',
  styleUrl: './edit-template.component.css'
})
export class EditTemplateComponent {
  @Output() submit = new EventEmitter<TemplateDto>();
  model: TemplateDto | null = null;
  visible = false
  Cardhashtags: string[] = ['[Nom Agence]', '[Numero de Carte]', '[Date Expiration]', '[Nom Client]', '[Prenom Client]'];
  Credithashtags: string[] = ['[Nom Agence]', '[Numero du Compte]','[Montant Mensuelle]','[Jour de paiement]',  '[Nom Client]', '[Prenom Client]'];
  private modalStateSubscription: Subscription | undefined;
  isDragging: boolean = false;

  constructor(private modalService: ModalService) {}

  ngOnInit(): void {
    this.reset();
  }

  ngOnDestroy(): void {
    if (this.modalStateSubscription) {
      this.modalStateSubscription.unsubscribe();
    }
  }

  onSubmit() {
    if (this.model) {
      this.submit.emit(this.model);
      this.modalService.closeModal();
      this.visible = false
    }
  }

  closeModal() {
    this.modalService.closeModal();
    this.visible = false
  }

  insertHashtag(hashtag: string) {
    if (!this.isDragging && this.model && this.model.templateText) {
      const textarea = document.getElementById('templateText') as HTMLTextAreaElement;
      const startPos = textarea.selectionStart;
      const endPos = textarea.selectionEnd;
      const textBeforeCursor = this.model.templateText.substring(0, startPos);
      const textAfterCursor = this.model.templateText.substring(endPos);
      this.model.templateText = textBeforeCursor + `#${hashtag} ` + textAfterCursor;
      textarea.focus();
      textarea.setSelectionRange(startPos + hashtag.length + 2, startPos + hashtag.length + 2);
    }
  }


  onDragStart(event: DragEvent, hashtag: string) {
    event.dataTransfer!.setData('text/plain', `#${hashtag} `);
    this.isDragging = true;
  }

  onDragEnd() {
    this.isDragging = false;
  }

  reset() {
    this.modalStateSubscription = this.modalService.getModalState().subscribe((modalData: any) => {
      if (modalData && modalData.data.model) {
        this.model = modalData.data.model;
        this.visible = true
      }
    });
  }

  getCategoryHashtags(): string[] {
    return this.model?.categoryId === 1 ? this.Cardhashtags : this.Credithashtags;
  }

}
