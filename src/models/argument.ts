export class ArgumentManifest {
  number: number;
  title: string;
  premises: ArgumentPremise[];
}

export class ArgumentPremise {
  number: number;
  title: string;
  description: string;
}

export const ConceptStatementArguments: ArgumentManifest[] = [
  {
    number: 1,
    title: "automation requires conflict",
    premises: [
      {number: 1, title: "title", description: "something"},
      {number: 2, title: "title", description: "something"}
    ] 
  },
  {
    number: 2,
    title: "automated architecture is not posthuman",
    premises: [
      {number: 1, title: "title", description: "something"}
    ]
  },
];

export const ProjectStatementArguments: ArgumentManifest[] = [
  {
    number: 1,
    title: "automation requires conflict",
    premises: [
      {number: 1, title: "title", description: "something"},
      {number: 2, title: "title", description: "something"}
    ] 
  },
  {
    number: 2,
    title: "automated architecture is not posthuman",
    premises: [
      {number: 1, title: "title", description: "something"}
    ]
  },
];