export interface Feat {
  title: string;
  description: string;
  icon: string;
}

export interface AboutItem {
  title: string;
  subtitle: string;
  imageUrl: string;
  component: any;
  feats: Feat[];
}
