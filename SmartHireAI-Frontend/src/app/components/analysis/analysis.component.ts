import { Component, OnInit } from '@angular/core';
import { ApiService, Job, MatchAnalysisResponse, ResumeListItem } from '../../services/api.service';

@Component({
  selector: 'app-analysis',
  templateUrl: './analysis.component.html',
  styleUrls: ['./analysis.component.scss']
})
export class AnalysisComponent implements OnInit {
  jobs: Job[] = [];
  resumes: { id: number; label: string }[] = [];
  selectedResumeId: number | null = null;
  selectedJobId: number | null = null;
  result: MatchAnalysisResponse | null = null;
  loading = false;
  error = '';

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.api.getJobs().subscribe({ next: (j) => (this.jobs = j) });
    this.loadResumeOptions();
  }

  loadResumeOptions() {
    this.api.getResumes().subscribe({
      next: (r) => {
        this.resumes = r.map(x => ({ id: x.id, label: `${x.candidateName} - ${x.fileName} (ID: ${x.id})` }));
      }
    });
  }

  runAnalysis() {
    const resumeId = this.selectedResumeId;
    if (!resumeId || !this.selectedJobId) {
      this.error = 'Select resume and job.';
      return;
    }
    this.loading = true;
    this.error = '';
    this.result = null;
    this.api.analyzeMatch({ resumeId, jobId: this.selectedJobId }).subscribe({
      next: (r) => { this.result = r; this.loading = false; },
      error: (e) => { this.error = e.error?.message || 'Analysis failed.'; this.loading = false; }
    });
  }
}
