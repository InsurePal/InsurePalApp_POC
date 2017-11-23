$.notify.addStyle("neoNotification", {
    html: "<div>\n<span data-notify-text></span>\n</div>",
    classes: {
        base: {
            "font-weight": "bold",
            "padding": "8px 15px 8px 14px",
            "text-shadow": "0 1px 0 rgba(255, 255, 255, 0.5)",
            "background-color": "#f5f5f5",
            "border": "1px solid #999999",
            "border-radius": "4px",
            "padding-left": "25px",
            "background-repeat": "no-repeat",
            "background-position": "3px 7px",
            "width": "186px",
            "color": "#999999"
        },
        error: {
            "color": "#B94A48",
            "background-color": "#F2DEDE",
            "border-color": "#B94A48",
            "background-image": "url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAVFJREFUeNpi/P//PwM1ARMDlQELPsld3p7SQMoUiBWgQg+A+LTb1u1PcelhxOZloEFiQKoeiCOAWAhN+h0QrwDiRqDBrwgaCDRMG0itBWJ1qNA/IP4GZXMhBdNNIA4GGnoVZxgCDRMFUhuQDGOA8jWheAOSOEjNBqgenJHSCMQqaGLfgK54AsJA9nc0ORWoHkwDgTZJAakYLOHMiYMNAzFAvZLYXGgBxLxYNHDgYMMASI8lNgMVcKQENhxsBmx6iUnYzMSmW3QF93Go0QKGUSeUrYlDzX1sLjwBxJ+xJX4gZoVibOAzVC+qgcBk8RxILcai4RpQrgiEQWws8ouherGGSQMQuwOxMpIYP9DLcjA2mvq7UD14s54ONOupIQl/hdLcSGK3oFnvCt7iC6rAFoinAvFbJINghr2FytmiG4aztEHLPdiKr2ckFV+DqsQGCDAA4QFnbYdAgAYAAAAASUVORK5CYII=')"
        },
        success: {
            "color": "#468847",
            "background-color": "#DFF0D8",
            "border-color": "#468847",
            "background-image": "url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAWhJREFUeNpi/P//PwM1ARMDlQELPkn3Tg9pIGUKxApQoQdAfHpn+Y6nuPQwYvMy0CBRIFUPxJFALIQm/Q6IlwNxI9Dg1wQNBBqmDaTWArE6Ad/dBOJgoKFXcRoIddlRIFYlMshuA7E1skvRI6WRSMM+Q2mQ2gassQx0nSSQiiHCsBVArAfEi4EY5L0YqF4MF1oAMS8S/yQWw1YBcRzQi6DYrgPiX0DMB9WLYSAsafwF4kKgJpCiMjTDYoDiv4EuEgCy5wIxO5perAkbZOAREAOouRuafNagGbYBiJ0IJewHUJoNpAGo0RdowHkgbkIKZ5hh9mjmPMDmwhNA/AnKBuWQrUADDIkw7BNUL9Z0OAVIZSMpfg4SBuL7QLwFi2EgMBXoixx86fAOEh+UHHYD8Skcht2B6qFd1sOIZagCOyAGef8tFoPeQuXs0A3DWdoguVYKSJlA0xkjNCzPAA16RlLxNahKbIAAAwB5F4hwFp+vfAAAAABJRU5ErkJggg==')"
        },
        info: {
            "color": "#3A87AD",
            "background-color": "#D9EDF7",
            "border-color": "#3A87AD",
            "background-image": "url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAX1JREFUeNpi/P//PwM1ARMDlQELPknrjnXSQMoMiJWgQveA+NTRiqCnuPQwYvMy0CAxIFUPxBFALIQm/Q6IVwBxI9DgVwQNBBqmDaTWAbEaVOgiEEdC2cuBWB/KvgXEQUBDr+IMQ6BhokBqA5JhIPABqOk6CIPYSOIgNRugenCGYSMQq6CJ2QM1nQbSIK9oo8mpQPVkYXgZqEkKSN0AYl4kDeeAeC8Qc0J9EwLEYmiGfgZiDaAPnqG70ALNMBDYAFTYjBa+6AbyQvWuQw9DBSypwBJoiCMSnxlHalEgKh0CgSfURSbk5JQHONQgpysOHGruYzPwODSAkQEo4Z5F4p+FiqFHygkMA4GB/xxILUFTPA8onoGkBsSeh6ZmCVQv1jBsAGI3IFaG8oWBkSIDSl5I3hdGUn8XmkXxZj0dILUWKbd8RbOUGynrBQNddwVv8QVVYAvEU4H4PdQAZPweKmeLbhjO0gbJtaDcY4qUzkAp4TQsVxBdfA2qEhsgwACIp3q05qcXlQAAAABJRU5ErkJggg==')"
        },
        warning: {
            "color": "#C09853",
            "background-color": "#FCF8E3",
            "border-color": "#C09853",
            "background-image": "url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAATpJREFUeNpi/P//PwM1ARMDlQFRBh6cGRIDxNHEqGUk5GWgQZZA6hiUa2WfvuY4pS5sQWI3U+RloOt8gZQTkpAzVIx0LwM1sgKp00CsDsQXoMIGQHwTiM2AXv9FqgvjgFgfiK8DNVuCMJB9AyoWS5KXga7jA1I1UC4LkhSMXQNVQ7QLc4BYAcrmgFrCCGND5XKICkOgRkkgdRmIhaFCT4HYGGr5OSCWgIq/BWJdYFA8J+TCCiTDQEAKiM8D8VkgFkcSF4aqxe1CoOs0oK7gRFIDckkQlL0OzbLvQGwEdOUNXC5sQDMMBH4ANRwCYRAbTY4TqgfThUDX2QOpfVgs+Q3Eh6BsOyBmRZP/B0r8QAsPoicJSxxhCjLAmUBuswC5Cd3AWUD8EYi5SSyxvgLxSqJLm6FRwJICAAIMAKNNXpJlOWlhAAAAAElFTkSuQmCC')"
        }
    }
});

function NeoNotification(text, type) {
    $.notify(text, { clickToHide: true, /*autoHide: false,*/ position: "left bottom", style: "neoNotification", className: type });
}