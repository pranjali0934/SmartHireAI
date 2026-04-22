import { Component, OnInit } from '@angular/core';
import { ApiService, Job, RankedCandidate } from '../../services/api.service';

@Component({
  selector: 'app-candidate-ranking',
  templateUrl: './candidate-ranking.component.html',
  styleUrls: ['./candidate-ranking.component.scss']
})
export class CandidateRankingComponent implements OnInit {
  jobs: Job[] = [];
  selectedJobId: number | null = null;
  candidates: RankedCandidate[] = [];
  loading = false;

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.api.getJobs().subscribe({ next: (j) => (this.jobs = j) });
  }

  onJobSelect() {
    if (!this.selectedJobId) return;
    this.loading = true;
    this.api.getRankedCandidates(this.selectedJobId).subscribe({
      next: (c) => { this.candidates = c; this.loading = false; },
      error: () => { this.loading = false; }
    });
  }

  scoreColor(score: number): string {
    if (score >= 70) return '#4caf50';
    if (score >= 40) return '#ff9800';
    return '#f44336';
  }
}
