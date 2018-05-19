import * as React from 'react';

import { Redirect, Route } from 'react-router';
import { ConnectedRouter } from 'react-router-redux';

import './App.css';
import Items from './items/items.container';
import Login from './login/login.container';
import { history } from './store';


class App extends React.Component {

  public render() {

    const redirectToLogin = () => (<Redirect to="/login" />);

    return (
      <ConnectedRouter history={history}>
        <div>
          <Route path="/login" component={Login} />
          <Route path="/items" component={Items} />
          <Route exact={true} path="/" render={redirectToLogin} />
        </div>
      </ConnectedRouter>
    );
  }
}

export default App;
