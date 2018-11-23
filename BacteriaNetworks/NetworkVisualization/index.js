$("document").ready(function(){
    var genes = JSON.parse(getUrlParameter('genes'));
    var cliques = JSON.parse(getUrlParameter('cliques'));
    var nodes = new vis.DataSet();
    var edges = new vis.DataSet();

    genes.forEach(gene => {
        nodes.add({id:gene, label:gene.toString()});
    });

    for(let k = 0; k < cliques.length;k++)
    {
        let clique = cliques[k];
        for(let i = 0; i < clique.length;i++)
        {
            for(let j = i + 1; j < clique.length;j++)
            {
                edges.add({from:clique[i], to:clique[j]});
                nodes.update({id: clique[j], group: k});
                nodes.update({id: clique[i], group: k});
            }
        }
    }

    // create a network
    var container = document.getElementById('bacteriaNetwork');

    // provide the data in the vis format
    var data = {
        nodes: nodes,
        edges: edges
    };
    var options = {
        nodes: {
            shape: 'dot',
            size: 30,
            font: {
                size: 32,
                color: '#000'
            },
            borderWidth: 2
        },
        edges: {
            color:{
                inherit: true
            },
            width: 2
        }
    };

    // initialize your network!
    var network = new vis.Network(container, data, options);
});


var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
};