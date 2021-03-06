﻿var addButton = document.querySelector('#add-users');
addButton.addEventListener(
    'click',
    function(e) {
        var target = e.target;
        var toAdd = userContainer.querySelectorAll('div.badge');
        if (toAdd && toAdd.length > 0) {
            var users = [];
            for (var i = 0; i < toAdd.length; ++i) { users.push(toAdd[i].innerText); }

            var jqxhr = new JQueryXhr();
            jqxhr.send(
                target.dataset.url, 
                function (data) {
                    $('<div></div>').html(data).dialog({ width: 'auto', modal: true, title: 'Users Added' });
                },
                { users: users }
            );
        }
    }
    , false
);

var userContainer = document.querySelector('#user-list').firstElementChild;
userContainer.addEventListener(
    'click',
    function(e) {
        var target = e.target;
        var tag = target.tagName.toLowerCase();
        if (tag === 'span') { removeFromDom(target.parentElement) }
    }
    , false
);

function addToDom(id, name, office) {
    if (!userContainer.querySelector('[data-id="' + id + '"]')) {
        var div = document.createElement('div');
        div.innerHTML = "<div class='badge small progress-bar-success' data-id='" + id + "'>"
            + name + ' - ' + office
            + " <span class='glyphicon glyphicon-remove text-danger' style='font-size:1.08em;' role='button'></span></div>";
        userContainer.appendChild(div);
    }
}

function removeFromDom(element) {
    element.parentElement.removeChild(element);
}

new JQueryAutoComplete().autocomplete(
    '#searchText',
    // callback **MUST** name parameters **EXACTLY** same as below
    function(event, ui) {
        addToDom(ui.item.value, ui.item.label, ui.item.office)
        console.log('Selected label: ' + ui.item.label + ' value: ' + ui.item.value);
    }
);