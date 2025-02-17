
var _skGeolocatorModule = {
  $callbacks: {
    onDynamicCall: {}
  },

  
      SK_Geolocator_init: function(gameObjNameStr, onDynamicCall) {
        const gameObjName = UTF8ToString(gameObjNameStr);
        callbacks.onDynamicCall = onDynamicCall;
        window._skGeolocator = (function () {
  'use strict';

function UCALL(funcName, arg) {
      if(!window._unityInstance){
        console.log("Unity game instance could not be found. Please modify your index.html template.");
        return;
      }
      
      window._unityInstance.SendMessage(gameObjName, funcName, arg);
    }
      

      function DYNCALL(funcName, payload, data) {
        if (!(payload instanceof String)) {
          payload = JSON.stringify(payload);
        }

        if(!data) {
          data = new Uint8Array();
        }
    
        const payloadBufferSize = lengthBytesUTF8(payload) + 1;
        const payloadBuffer = _malloc(payloadBufferSize);
        stringToUTF8(payload, payloadBuffer, payloadBufferSize);
    
        const funcNameBufferSize = lengthBytesUTF8(funcName) + 1;
        const funcNameBuffer = _malloc(funcNameBufferSize);
        stringToUTF8(funcName, funcNameBuffer, funcNameBufferSize);
    
        const buffer = _malloc(data.length * data.BYTES_PER_ELEMENT);
        HEAPU8.set(data, buffer);
    
        Module.dynCall_viiiiii(
          callbacks.onDynamicCall,
          funcNameBuffer,
          funcNameBufferSize,
          payloadBuffer,
          payloadBufferSize,
          buffer,
          data.length
        );
    
        _free(payloadBuffer);
        _free(funcNameBuffer);
        _free(buffer);
      }
      

  class Geolocator {
      watch(options) {
          options = this.cleanOptions(options);
          console.log(options);
          const watchId = navigator.geolocation.watchPosition((success) => {
              DYNCALL("OnWatchPosition", this.createResult(success, watchId), null);
          }, (error) => {
              DYNCALL("OnWatchError", this.createError(error), null);
          }, options);
          return watchId;
      }
      clearWatch(watchId) {
          navigator.geolocation.clearWatch(watchId);
      }
      getCurrentPosition(options) {
          options = this.cleanOptions(options);
          console.log(options);
          navigator.geolocation.getCurrentPosition((success) => {
              DYNCALL("OnCurrentLocation", this.createResult(success), null);
          }, (error) => {
              DYNCALL("OnCurrentLocationError", this.createError(error), null);
          }, options);
      }
      cleanOptions(options) {
          if (!options) {
              return null;
          }
          options.timeout = options.timeout == 0 ? null : options.timeout;
          options.maximumAge = options.maximumAge == 0 ? null : options.maximumAge;
          return options;
      }
      createResult(success, watchId = 0) {
          return {
              watchId: watchId,
              coords: {
                  accuracy: success.coords.accuracy,
                  altitude: success.coords.altitude,
                  altitudeAccuracy: success.coords.altitudeAccuracy,
                  heading: success.coords.heading,
                  latitude: success.coords.latitude,
                  longitude: success.coords.longitude,
                  speed: success.coords.speed,
              },
              timestamp: success.timestamp,
          };
      }
      createError(error) {
          return {
              message: error.message,
              code: error.code,
              PERMISSION_DENIED: error.PERMISSION_DENIED,
              POSITION_UNAVAILABLE: error.POSITION_UNAVAILABLE,
              TIMEOUT: error.TIMEOUT,
          };
      }
  }

  class UnityHooks {
      static isAvailable() {
          return navigator != undefined && navigator.geolocation != undefined;
      }
      static watch(options) {
          return UnityHooks.geolocator.watch(options);
      }
      static clearWatch(watchId) {
          UnityHooks.geolocator.clearWatch(watchId);
      }
      static getCurrentPosition(options) {
          UnityHooks.geolocator.getCurrentPosition(options);
      }
  }
  UnityHooks.geolocator = new Geolocator();

  return UnityHooks;

})();

      },
    SK_Geolocator_isAvailable: function() {
var result = window._skGeolocator.isAvailable();
var bs = lengthBytesUTF8(result);
var buff = _malloc(bs);
stringToUTF8(result, buff, bs);
return buff;
},
SK_Geolocator_watch: function(options) {
return window._skGeolocator.watch(JSON.parse(UTF8ToString(options)));
},
SK_Geolocator_clearWatch: function(watchId) {
window._skGeolocator.clearWatch(watchId);
},
SK_Geolocator_getCurrentPosition: function(options) {
window._skGeolocator.getCurrentPosition(JSON.parse(UTF8ToString(options)));
},

};

autoAddDeps(_skGeolocatorModule, "$callbacks");
mergeInto(LibraryManager.library, _skGeolocatorModule);
