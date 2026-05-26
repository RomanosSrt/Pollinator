
// import './App.css'
import { MapContainer, Marker, Popup, TileLayer } from 'react-leaflet';
import MarkerClusterGroup from 'react-leaflet-markercluster';
import 'leaflet/dist/leaflet.css';
import { LatLng, point, divIcon } from 'leaflet';

interface marker {
  position: LatLng;
  popupText: string;
}

interface Cluster {
  getChildCount: () => number;
}

export function App() {
  const markers : marker[] = [
    {position: new LatLng(37.971863, 23.727871), 
    popupText: "Hello! I'm in Athens." },
    {position: new LatLng(37.937684, 23.635360), 
    popupText: "Hello! I'm in Piraeus." },
    {position: new LatLng(38.003919, 23.824152), 
    popupText: "Hello! I'm Home." }
  ];
  
  const createClusterCustomIcon = function (cluster : Cluster)  {
    return divIcon({
      html: `<span>${cluster.getChildCount()}</span>`,
      className: 'bg-blue-500 h-2 w-2 justify-center align-middle rounded-xl ring-1',
      iconSize: point(40, 40, true),
  });
}
  return (
    <MapContainer center={markers[0].position} zoom={13} style={{position: 'fixed', top: '10%', left: '10%', height: '80%', width: '80%' }}>
      {/* TileLayer displays the actual map tiles from OpenStreetMap */}
      <TileLayer
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />

      {/* Marker is a pin on the map */}
      <MarkerClusterGroup chunkedLoading iconCreateFunction={createClusterCustomIcon}>
      {markers.map((marker) => (
          <Marker position={marker.position} >
            <Popup>
              <div>{marker.popupText}</div>
            </Popup>
          </Marker>
      ))}
      </MarkerClusterGroup>

    </MapContainer>
  );
} export default App;