export class ArgumentManifest {
  number: string;
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
    number: "A >",
    title: "they worship machines",
    premises: []
  },
  {
    number: "1 :",
    title: "on interface",
    premises: [
      {number: 1, title: "a definition for the black box", description: "something"},
      {number: 2, title: "a definition for conflict", description: "something"},
      {number: 3, title: "automation requires conflict", description: "something"}
    ] 
  },
  {
    number: "2 :",
    title: "on productive conflict",
    premises: [
      {number: 1, title: "architectural practice is a black box", description: "something"},
      {number: 2, title: "architecture can be automated", description: "something"},
      {number: 3, title: "exigence", description: "something"},
      {number: 4, title: "an automated architecture is not posthuman", description: "something"},
    ]
  },
  {
    number: "3 :",
    title: "on tools",
    premises: [
      {number: 1, title: "input without pedigree", description: "something"},
      {number: 2, title: "architects that make tools", description: "something"},
      {number: 3, title: "tools that make tools", description: "something"},
      {number: 4, title: "tools that make architects", description: "something"},
    ]
  },
  {
    number: "B >",
    title: "architecture with/out architects",
    premises: []
  },
  {
    number: "4 :",
    title: "on the recursive public",
    premises: [
      {number: 1, title: "an appeal", description: "something"},
      {number: 2, title: "a demand", description: "something"},
    ]
  },
  {
    number: "5 :",
    title: "on implementation",
    premises: [
      {number: 1, title: "via the smart city", description: "something"},
      {number: 2, title: "via the old city", description: "something"},
      {number: 3, title: "via something in between", description: "something"}
    ]
  }
];

export const ProjectStatementArguments: ArgumentManifest[] = [
  {
    number: "1",
    title: "automation requires conflict",
    premises: [
      {number: 1, title: "title", description: "something"},
      {number: 2, title: "title", description: "something"}
    ] 
  },
  {
    number: "2",
    title: "automated architecture is not posthuman",
    premises: [
      {number: 1, title: "title", description: "something"}
    ]
  },
];