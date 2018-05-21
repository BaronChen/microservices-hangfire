import App from './App';
import { configureStore } from './store';
import { storeRegistry } from './store-registry';

import './index.css';

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';

const store = configureStore();
storeRegistry.registerStore(store);

ReactDOM.render(
  <Provider store={store}>
    <App />
  </Provider>,
  document.getElementById('root') as HTMLElement
);
// registerServiceWorker();
