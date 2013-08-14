/*
 * jQuery UI addresspicker @VERSION
 *
 * Copyright 2010, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Progressbar
 *
 * Depends:
 *   jquery.ui.core.js
 *   jquery.ui.widget.js
 *   jquery.ui.autocomplete.js
 */
(function ($, undefined) {

    $.widget("ui.addresspicker", {
        options: {
            appendAddressString: "",
            mapOptions: {
                zoom: 12,
                center: new google.maps.LatLng(46, 2),
                scrollwheel: false,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            },
            elements: {
                map: false,
                lat: false,
                lng: false,
                locality: false,
                country: false
            },
            draggableMarker: true
        },

        marker: function () {
            return this.gmarker;
        },

        map: function () {
            return this.gmap;
        },

        updatePosition: function () {
            this._updatePosition(this.gmarker.getPosition());
        },

        reloadPosition: function () {
            this.gmarker.setVisible(true);
            this.gmarker.setPosition(new google.maps.LatLng(this.lat.val(), this.lng.val()));
            this.gmap.setCenter(this.gmarker.getPosition());
        },

        selected: function () {
            return this.selectedResult;
        },

        _create: function () {
            this.geocoder = new google.maps.Geocoder();
            this.element.autocomplete({
                source: $.proxy(this._geocode, this),
                focus: $.proxy(this._focusAddress, this),
                select: $.proxy(this._selectAddress, this)
            });

            this.lat = $(this.options.elements.lat);
            this.lng = $(this.options.elements.lng);
            this.locality = $(this.options.elements.locality);
            this.country = $(this.options.elements.country);
            if (this.options.elements.map) {
                this.mapElement = $(this.options.elements.map);
                this._initMap();
            }
        },

        _initMap: function () {
            if (this.lat && this.lat.val()) {
                this.options.mapOptions.center = new google.maps.LatLng(this.lat.val(), this.lng.val());
            }

            this.gmap = new google.maps.Map(this.mapElement[0], this.options.mapOptions);
            this.gmarker = new google.maps.Marker({
                position: this.options.mapOptions.center,
                map: this.gmap,
                draggable: this.options.draggableMarker
            });
            google.maps.event.addListener(this.gmap, 'click', $.proxy(this._mapClicked, this));
            google.maps.event.addListener(this.gmarker, 'dragend', $.proxy(this._markerMoved, this));
            this.gmarker.setVisible(false);
        },

        _updatePosition: function (location) {
            if (this.lat) {
                this.lat.val(location.lat());
            }
            if (this.lng) {
                this.lng.val(location.lng());
            }
        },

        _mapClicked: function (event) {
            this.gmarker.setPosition(event.latLng);
            this._markerMoved();
        },

        _markerMoved: function () {
            var self = this;
            var pos = this.gmarker.getPosition();
            this._updatePosition(pos);

            if (!this.element.val() || (this.country && !this.country.val())) {
                this._geocodeByField({ location: pos }, function (addresses) {
                    if (addresses && addresses.length > 0) {
                        self._setLocalityAndCountry(addresses[0]);
                    }
                });
            }
        },

        // Autocomplete source method: fill its suggests with google geocoder results
        _geocode: function (request, response) {
            var address = request.term;
            this._geocodeByField({ address: address + this.options.appendAddressString }, response);
        },

        _geocodeByField: function (request, response) {
            this.geocoder.geocode(request, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    for (var i = 0; i < results.length; i++) {
                        results[i].label = results[i].formatted_address;
                    };
                }
                response(results);
            })
        },

        _findInfo: function (result, type, nameType) {
            for (var i = 0; i < result.address_components.length; i++) {
                var component = result.address_components[i];
                if (component.types.indexOf(type) != -1) {
                    return component[nameType];
                }
            }
            return null;
        },

        _focusAddress: function (event, ui) {
            var address = ui.item;
            if (!address) {
                return;
            }

            if (this.gmarker) {
                this.gmarker.setPosition(address.geometry.location);
                this.gmarker.setVisible(true);

                this.gmap.fitBounds(address.geometry.viewport);
            }
            this._updatePosition(address.geometry.location);

            this._setLocalityAndCountry(address);
        },

        _setLocalityAndCountry: function (address) {
            if (this.locality) {
                this.locality.val(this._findInfo(address, 'locality', 'long_name'));
            }
            if (this.country) {
                this.country.val(this._findInfo(address, 'country', 'short_name'));
            }
        },

        _selectAddress: function (event, ui) {
            this.selectedResult = ui.item;
        }
    });

    $.extend($.ui.addresspicker, {
        version: "@VERSION"
    });


    // make IE think it doesn't suck
    if (!Array.indexOf) {
        Array.prototype.indexOf = function (obj) {
            for (var i = 0; i < this.length; i++) {
                if (this[i] == obj) {
                    return i;
                }
            }
            return -1;
        }
    }

})(jQuery);
