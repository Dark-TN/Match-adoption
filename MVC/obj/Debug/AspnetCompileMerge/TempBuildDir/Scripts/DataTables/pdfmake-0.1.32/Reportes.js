function GenerarPDFSalida(folio, fecha, rs, entrega, recibe, destino, autoriza, justificacion, equipos) {
    var datos_equipos = [];
    for (var i = 0; i < equipos.length; i++) {
        var row = [];
        if ((i + 1) % 2 == 0) {
            row = [{ text: equipos[i].Id, bold: true, border: [false, false, false, false], fillColor: '#E6E6E6' },
                { text: equipos[i].Descripcion, bold: true, border: [false, false, false, false], fillColor: '#E6E6E6' },
                { text: equipos[i].Marca, bold: true, border: [false, false, false, false], fillColor: '#E6E6E6' },
                { text: equipos[i].NombreModelo, bold: true, border: [false, false, false, false], fillColor: '#E6E6E6' },
                { text: equipos[i].NoSerie, bold: true, border: [false, false, false, false], fillColor: '#E6E6E6' }];
        }
        else {
            row = [{ text: equipos[i].Id, bold: true, border: [false, false, false, false], fillColor: '#E7EBB6' },
                { text: equipos[i].Descripcion, bold: true, border: [false, false, false, false], fillColor: '#E7EBB6' },
                { text: equipos[i].Marca, bold: true, border: [false, false, false, false], fillColor: '#E7EBB6' },
                { text: equipos[i].NombreModelo, bold: true, border: [false, false, false, false], fillColor: '#E7EBB6' },
                { text: equipos[i].NoSerie, bold: true, border: [false, false, false, false], fillColor: '#E7EBB6' }];
        }
        datos_equipos.push(row);
    }
    var doc = {
        content:
            [
                {
                columns:
                    [
                        {
                            image: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAfQAAACWCAMAAAAfQ4pdAAABCFBMVEU9tFl0vVdVvW1IuGPO7NWe2ay248H////17k1tx4J5vlePxFbz+vWXx1WcyFS30VHo9uqF0JdquliAwFeLw1aDwVetzlJmuVja8eCHwlaTxVVhuFhhwnihylRYtlmky1SnzFNdt1iq3rZStVmxzlL+/vTC58tMtVl5y4yS1aBEtFn59Ituu1j8+9J9v1dwu1j271eqzFP796nd6Wn4/PWrzFL9/N2zz1Ls60/+/en7+bz38W728GP49Zru9M/38nnj6V3f7JXh636bzXXo8a/0+eqMyXnS5q3n9OBtw1f8+sfJ4JjF23vd5k7E3lDg79PM58Cy2Ziw03Pz+OCi1Iua0VNzv2Vrw3hSLE21AAAMFklEQVR42u3dC3uTSBcAYAoDFEWF9YYu6i5+xWCs20262lt6tfdq1VW///9PtknaJsCZC2RIBnKOWp+nD+H2MsNwZoZoNsbchYanANExEB0D0TEQHQPRMRAdA9ExEB0D0SXG0vLZ3t5+t7u7mIndbnd/r3e2vI7oDYrlT3t5aih2u3sby4hee29B7hT9fm8Z0WtanX/aL+w9Ln+2hOj1iu1JwMfgEb02Rby7tigrup+WEF158b3uouSYG/eaon/qLq4tVhDz4V5H9O1uJeDDWNv/gOjKxd7uYsWx2/TiXjP09c7K4hRibX8d0VUhby9OLfa3EV2FpNtE5Js77U6ns7E8iOMfp4N49vPnH1++UFoI3W1En3Up3ymHvbXT6ywD2dbYN7TbeHl69+eXz/mb+zai161i3+kxe1Vcy3l7Hdrg55vTnHz3X0SfVSKmV7gy722IPHl5iTHkfnv7358Pn6Tgu0uIPoso2GLf7BXpQrH+/yYTb9+8fvRgdJ9f20P06bfftgqAr7Q3CpdM0kqjvx78PB0V+LVtRJ9uzd4uUsRLJtO85DUUI/eG3doVRz8Trtk3O5PkU9LsL2/+e3l6Xc83q45XGn1J9DFtqzdxBs1LXoLx+/Nhcd/9F9FVKuZtOYOePB9mf3nnQcMKu7rognfzrQ15z1SXR3/CcfqkX8t3DxG92viwNcVCfhvxLwr70z772idEr/TZXOT5rCe/L8yNfqfEb332fUSfadW+0qkmVxYcsdh3DxG9oqp9U+BWXt3mrV9PKfHuSyMyNSqiL6/Mkrzfjj+iqT9d+NyAG7uC6BuzqtjHC/tvtHh67/MeossObo9aZxr9XidHNPV3Cw+6iC43eE249pRGr7k/3lHjec37WxVD5zXbt6Y44/Drr78o8W7hxyqiSzPf5NzMp7o3J0cLtPjraBXRp2K+M+1xye6Pvi/0d2GhzupabcynXMyH8W1h4Q709+pfjdW1upjvzGb6wdc71KivulYT886sduvyYfPU1UFntds3Zzip8OQxxfz9+wNEr868PdPH4pOD97T4huiTBCMPt7Ix431zDx5C8f5hXdUVQWfk27dmP198FVR/3P/xFdHLxgfG7VyFjOfqwWNaXCJ6uVin96W21ThNdPVHJ4gu+WGto8p5cg+eUeJgFdGlNtw31DlRJ3dp6t8QXWIjbmVDpTN1cvcRHM+OEb1wI452Q19R7DVPl49ocYLoBWOzJua2/fXuo8yf66jbbX3m6L3amNv2t7uUOEf0IrFcI3PbPqCpXyJ6gac12uQlNV/EvnrvORz1quBnjN5R/1kt3ZijoD8/R3ThlrvqOZlcHN+D4/kJogvGjtK5VzC+U9S/I/pEaZkdlU/Y4QuK+jGiC7Xi4LTMltozCY7vw/FiFdFLt+JWVH/f+neK+jGi82O9Xg33UQX/Cizo9+8fIjo32rVrxN1U8C/gOG82upONAF4uyS6X8Ao644ZuiW1zGhU8Rf2w0ehaNgjl4sgu5/AKOiMTZ4ptcxopmle5eHH159U5orPR4YLes+uAbl+8guMQ0Zno7cJPawqhH/4Bxzmis9CXwILO/PZLhdDtc4r6IaIz0DvFU3EqoR/+DcU/fx8jOgMd7FJdrw26ff4PGN8RnY7+ocQjulLoSx+fgIHodHSodl9ZrxG6vY3oRdHbJTrR1UK3z/8HBaLT0XdKdKgqhm5fIPqk6CtLdUNfvfiYiwtEL3BPF+hQVQ39Sv1BNvCRrUDrXWQSunLoQ/WPY/8eYHKG9ZzeLv5+EfXQbXsvVc4/1uQF0TNLw46pb50JbVRFdHv7onaV+yy7Vpfbm8NvwRWd16Ak+hj7eW2+xGt26IVDUfS++1XU6WvbEH0OA9ERXTV0QizTIkEpdJcQMyIBdc3ENC1CPLEdCQiJTJMQMvHIvGBwSMTlL+mRYXhzhE6S8PZzYRIUQ499/Xoxw886eVFLH98nkwMZ++H4dvVWVFbei0ZnRPdj+nJW4hhyNqkQeubXZv6jlp5ZRg/hbTr537mmkfpkesW5nbpawqQWPM83NOYHaJc1yR0gyW5Zt8AtJroGbtJrOHoMHrcgepRh8lnX0k0kILvr07Zu3LCLohPgYtOcHCS42M2BuA1Gp59qAXQvt9FRNRqE9NUZQG0bG4zt60EBdI9iaaSr7cBhHrIRNxadRcNFB5yo7X9eQeIsb4qjt6hXT0rd5B500lD0wNDKo1v55VrC9UeYVk/YSxuuODp/NYMQuKn56qCLhgC6qDmIDrlet5ZcgfojpW5pIgV9YvTxJxpLYPGogeieqDmIDoUnbJ5S5+2I7kpCH2tziBR1w2scuphNEfRwuJgvttLwdk9anCUtWxa6Xqyo+41DTzTZ6KZgEynTUvJEpSZHH28RCRR1zW0YeoEzJYo+aB0HhVebvfp8MsyMmmG6oMtA94sVdath6Lp09GGJFL9p3BThkHqivX6+T7cloo+nDPXbxLM1TLqTSJddv6uFDlznetRPuBAgNclB11vmVfgmZb2tiBASJwatINFZBmleqyS64zjUtuboFLSsVBXu09odjUDPyRqjBxTLKICuR6k2bm69YXDrB5e7gN1R5Nkl0K+7ftzIYLTfbR3oCNAp2aYmoFuF0lV0dCPzLGuxGsAW+AhF6BkU9rmgo48SfoHGyFNAT2RJg9EdZmJaGD3MnrdQY9WPFpTCI+xkXQl0i15fQ72MrHxwk9A9dvJJFD3n4zFuoeATOVQYdTIh+vjnY0qmGB5HYZq+3DFDKqFbGqv1JIqu58pkxGn9elATEWj6edLQA/7Ykuw4iqai++znUUH0/BCTFm+JFrA7UELOIZLQbQ468XXBZE7t0TMHathl0IFCYzArEEptG8NZWqt6dDfiZSsUQY8IGGEhdM6Nriy6y81tuNBeUvI5uSFOpdBD+k5b/ASVIugy+tMDTpO2LDrh903qwLALauY206QrhU7dabclUF82CJ1w1ikLnfD3k5MGT41sk4ruCeWLG4QeV4Qe8ffVh9AZ6uNj1WSiu4UGijUB3WQ/TZdGFxgvb4Lo+cwv9GQhE93R5h3dnjm67Tl8dYnokTb36GR66An1eqO3pQPp6C44rcJxHCdsLHpUEbrF6NKC99NId6PC6KF09OwJCJPYA9fV4Na7VVHr3eTuZ3odLqW0W7LRQ2r6b37Qk4rQHe7x5NI3lkPPLkpDd+mpqeaie+z+lvJp2OxOuCXqAmiKmSsXPaZ34DcX3dbYHSOl0XXOfSPXjgOPJp9TjuWim/RqrsHoDjv5Xhrd51Qh2R4Zg3LUvsilKgk9nhf0hL3W0ugWe3BGrtVMG9Lg6tCtXxq6Tz/0BqPHmoThUgC6qzFvHLnJc9T5wAm0KWnoDv26bDB6Hiel7pdFzw+IGF+vp1Nqf0I4aR6nwurdnxd0YLTK7bhW4GFZHB0Ygn47zDifXx+Nx3Ys5sVT5T091fOQzQY3Ch2aH2D4FiFRi56BFkAHh0n7ESFxks+73DwsDVoCqRe9kGobcummxWhIb9xqcO7dLjarqQh6kSlyVmZfQnP4+i/PhHdAGnr2A35MCImg9xw1Cz2uCN1uCa/VgZr8BjQZyZCdhi06M78h6KIdyoXRXdETejvln1/pmLLR/TlFF38PRTF04Qqe2LxRM9l7vzx0MqfoQvOzy6ALrtiyhQt6BKfzJhlE4TQbPaT2cPgVoReb7s9fuMXrli2BHiiNbkyKntC7tfyK0PmQY8MduQV9NGEukjhGTvAlKd5M0FuTomfu3aYtpm5Mgs5+/+PYnHWBUaljkyQzbcTJhkDTjr0VsjqMpoNOJkXPFLt0BzZtEKoROBOhs8Y5juXohvvALOupd0vGEtFhdSNKfSCeDXquqBd/9XdK1hTBabn2hOiMFw37ntBomWFJixk1yIToUHXUv8Qc+oiiqaFnZ9+UeN+7G42m4uaGqpDcDaRFbHtydNjSSMC7pBdBtbwe5UbeuJGjy0LPvbd8uG83Hwh9CV9oUfqtBkFkjgWlaWGZmRB/G5ZrjabrGq1I4rcbeFbmHf+M0+jGZmqWeJhM40tEYtMZ7GHI/f6BKaNPJYLB3Fe3gjUTQiJLdN393YgiCV/loUhoNsbcBaIjOgaiYyA6BqJjIDoGomMgOoY68R8E8J9KVWdZ3QAAAABJRU5ErkJggg==",
                            width: 125,
                            height: 40
                        },
                        {
                            width: 280,
                            text: 'Salida de equipo y perférico de cómputo',
                            fontSize: 15,
                            color: '#149F45',
                            bold: true
                        },
                        {
                            table: {
                                headerRows: 1,
                                widths: [60],
                                body: [
                                    [{ text: 'Folio', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#149F45' }],
                                    [{ text: folio, bold: true, border: [false, false, false, false], fillColor: '#E6E6E6' }]
                                ]
                            }
                        }
                    ]
            },
                {
                    table: {
                        widths: ['*'],
                        body: [[" "]]
                    },
                    layout: {
                        hLineWidth: function (i, node) {
                            return (i === 0 || i === node.table.body.length) ? 0 : 0;
                        },
                        vLineWidth: function (i, node) {
                            return 0;
                        },
                    }
                },
                {
                table: {
                    headerRows: 1,
                    widths: [85, 413],
                    body: [
                        [{ text: 'Fecha', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#149F45' },
                            { text: fecha, bold: true, border: [false, false, false, false], fillColor: '#E6E6E6' }],
                        [{ text: 'Razón Social', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#149F45' },
                            { text: rs, bold: true, border: [false, false, false, false], fillColor: '#E6E6E6' }],
                        [{ text: 'Entrega', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#149F45' },
                            { text: entrega, bold: true, border: [false, false, false, false], fillColor: '#E6E6E6' }],
                        [{ text: 'Recibe', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#149F45' },
                            { text: recibe, bold: true, border: [false, false, false, false], fillColor: '#E6E6E6' }],
                        [{ text: 'Destino', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#149F45' },
                            { text: destino, bold: true, border: [false, false, false, false], fillColor: '#E6E6E6' }]
                    ]
                    }
                },
                {
                    table: {
                        widths: ['*'],
                        body: [[" "]]
                    },
                    layout: {
                        hLineWidth: function (i, node) {
                            return (i === 0 || i === node.table.body.length) ? 0 : 0;
                        },
                        vLineWidth: function (i, node) {
                            return 0;
                        },
                    }
                },
                {
                    table: {
                        headerRows: 1,
                        widths: [506],
                        body: [
                            [{ text: 'Descripción del equipo', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#AFC828', alignment: 'center' }]
                        ]
                    }

                },
                {
                    table: {
                        headerRows: 1,
                        widths: [90, 100, 90, 90, 100],
                        body: [
                            [{ text: 'Folio', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#149F45' },
                                { text: 'Descripcion', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#149F45' },
                                { text: 'Marca', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#149F45' },
                                { text: 'Modelo', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#149F45' },
                                { text: 'No. Serie', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#149F45' }
                            ],
                            
                        ]
                    }
                },//datos de los equipos'							
                {
                    table: {
                        headerRows: 1,
                        widths: [90, 100, 90, 90, 100],
                        body: datos_equipos                       
                    }
                },//fin datos de los equipos
                {
                    table: {
                        headerRows: 1,
                        widths: [506],
                        body: [
                            [{ text: 'Justificación de salida de equipo y periférico de cómputo', bold: true, border: [false, false, false, false], color: 'white', fillColor: '#AFC828', alignment: 'center' }],
                            [{ text: justificacion, bold: true, border: [false, false, false, false], fillColor: '#E6E6E6' }]
                        ]
                    }

                },
                {
                    table: {
                        widths: ['*'],
                        body: [[" "],[" "]]
                    },
                    layout: {
                        hLineWidth: function (i, node) {
                            return (i === 0 || i === node.table.body.length) ? 0 : 0;
                        },
                        vLineWidth: function (i, node) {
                            return 0;
                        },
                    }
                },
                { //border: [izquierda, arriba, derecha, abajo]
                    table: {
                        headerRows: 1,
                        widths: [195,100,195],
                        body: [
                            [{ text: autoriza, border: [false, false, false, true], alignment: 'center'},
                                { text: '', border: [false, false, false, false] },
                                { text: entrega, border: [false, false, false, true], alignment: 'center' }],
                            [{ text: 'Autorización de soporte técnico', bold: true, border: [false, false, false, false], alignment: 'center' },
                                { text: '', bold: true, border: [false, false, false, false] },
                                { text: 'Entrega', bold: true, border: [false, false, false, false], alignment: 'center' }]
                        ]
                    }

                },
                {
                    table: {
                        widths: ['*'],
                        body: [[" "], [" "]]
                    },
                    layout: {
                        hLineWidth: function (i, node) {
                            return (i === 0 || i === node.table.body.length) ? 0 : 0;
                        },
                        vLineWidth: function (i, node) {
                            return 0;
                        },
                    }
                },
                { //border: [izquierda, arriba, derecha, abajo]
                    table: {
                        headerRows: 1,
                        widths: [195, 100, 195],
                        body: [
                            [{ text: recibe, border: [false, false, false, true], alignment: 'center' },
                            { text: '', border: [false, false, false, false] },
                            { text: '', border: [false, false, false, true], alignment: 'center' }],
                            [{ text: 'Recibe', bold: true, border: [false, false, false, false], alignment: 'center' },
                            { text: '', bold: true, border: [false, false, false, false] },
                            { text: 'Conocimiento de vigilancia', bold: true, border: [false, false, false, false], alignment: 'center' }]
                        ]
                    }

                }
            ]
    }
    pdf = pdfMake.createPdf(doc);
    pdf.open();
    pdf.download(folio + '_' + fecha + '.pdf');
}