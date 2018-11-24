export class BoxManifest {
    name: string;
    version: string;
    short_desc: string;
    long_desc: string[];

    iconRoute: string;
    tutorialRoute: string;
}

export class BoxManifestCategory {
    name: string;
    boxes: BoxManifest[];
}

export const AllBoxes: BoxManifestCategory[] = [
    {
        name: "key press",
        boxes: [
            { 
                name: "cicero",
                version: "0.0.5",
                short_desc: "placeholder",
                long_desc: [
                    "placeholder",
                    "placeholder"
                ],

                iconRoute: "",
                tutorialRoute: ""
            },
            {
                name: "seneca",
                version: "0.0.5",
                short_desc: "placeholder",
                long_desc: [
                    "placeholder",
                    "placeholder"
                ],

                iconRoute: "",
                tutorialRoute: ""
            }
        ]
    },
    {
        name: "black mirror",
        boxes: [
            {
                name: "theano",
                version: "0.0.5",
                short_desc: "placeholder",
                long_desc: [
                    "placeholder",
                    "placeholder"
                ],

                iconRoute: "",
                tutorialRoute: ""
            }
        ]
    }
];