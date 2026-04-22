import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService, Job } from '../../services/api.service';

@Component({
  selector: 'app-jobs',
  templateUrl: './jobs.component.html',
  styleUrls: ['./jobs.component.scss']
})
export class JobsComponent implements OnInit {
  form: FormGroup;
  jobs: Job[] = [];
  loading = false;

  constructor(private fb: FormBuilder, private api: ApiService) {
    this.form = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      requiredSkills: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.loadJobs();
  }

  loadJobs() {
    this.api.getJobs().subscribe({ next: (j) => (this.jobs = j) });
  }

  submit() {
    if (!this.form.valid) return;
    this.loading = true;
    this.api.createJob(this.form.value).subscribe({
      next: () => {
        this.form.reset();
        this.loadJobs();
        this.loading = false;
      },
      error: () => { this.loading = false; }
    });
  }
}
