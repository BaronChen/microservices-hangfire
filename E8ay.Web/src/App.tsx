import * as React from 'react';

import { Redirect, Route } from 'react-router';
import { ConnectedRouter } from 'react-router-redux';

import './App.css';
import Items from './items/items';
import Login from './login/login';
import { history } from './store';

import Alert from 'react-s-alert';
import 'react-s-alert/dist/s-alert-default.css';
import 'react-s-alert/dist/s-alert-css-effects/slide.css';

class App extends React.Component {

  public render() {

    const redirectToLogin = () => (<Redirect to="/login" />);

    return (
      <div className="app-root">
        <Alert stack={{limit: 3}} />
        <ConnectedRouter history={history}>
          <div className="main-container">
            <Route path="/login" component={Login} />
            <Route path="/items" component={Items} />
            <Route exact={true} path="/" render={redirectToLogin} />
          </div>
        </ConnectedRouter>
      </div>
    );
  }
}

export default App;
