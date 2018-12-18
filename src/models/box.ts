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
        name: "black box",
        boxes: [
            {
                name: "cicero",
                version: "1.1.0",
                short_desc: "",
                long_desc: [
                    "",
                    ""
                ],
                iconRoute: "",
                tutorialRoute: ""
            }
        ]
    },
    {
        name: "black swan",
        boxes: [
            { 
                name: "ersatz",
                version: "0.0.1",
                short_desc: "placeholder",
                long_desc: [
                    "placeholder",
                    "placeholder"
                ],

                iconRoute: "",
                tutorialRoute: ""
            },
            {
                name: "theano",
                version: "0.6.0",
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
                name: "wander",
                version: "0.0.3",
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