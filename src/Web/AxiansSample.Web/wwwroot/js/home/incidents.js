class Incidents {
    constructor() {
        var self = this;
        this.viewModel = {
            connectionReady: ko.observable(false),
            incidents: ko.observableArray([]),
            seachText: ko.observable(""),
            errorText: ko.observable(""),
            fetchIncidents: () => {
                get(`/incidents/getIncidents?seachText=${this.viewModel.seachText()}`)
                    .then(result => {
                        if (result.error) {
                            this.viewModel.errorText(result.error);
                        }
                        else {
                            this.viewModel.incidents(result);
                        }
                    });
            },
            generateSomeIncidents: () => {
                self.connection.invoke("generateSomeIncidents");
            },

        };

        this.viewModel.filteredItems = ko.computed(function () {
            const filter = this.viewModel.seachText().toLowerCase();
            const filteredItems = this.viewModel
                .incidents()
                .filter(i => {
                    const exists = filter === "" ||
                        i.ciid.toLowerCase().indexOf(filter) >= 0 ||
                        i.errorCode.toLowerCase().indexOf(filter) >= 0 ||
                        i.description.toLowerCase().indexOf(filter) >= 0;
                    return exists;
                });
            return filteredItems;

        }, this);

        ko.applyBindings(this.viewModel, document.getElementById("incidents-scope"));
        this.viewModel.fetchIncidents();

        this.connection = new signalR.HubConnectionBuilder().withUrl("/incidentHub").build();
        this.connection.start().then(function () {
            self.viewModel.connectionReady(true);
        }).catch(function (err) {
            this.viewModel.errorText(`SignalR error: ${err.toString() }`);
        });
        this.connection.on("updatedIncidents", (incidents) =>{
            self.viewModel.incidents(incidents);
        });
    }
}