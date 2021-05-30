import React, { Component } from 'react';
import { Route } from 'react-router';
import Layout from './Layout';
import Generate from './Pages/Generate';
import Home from './Pages/Home';
import Upload from './Pages/Upload';


export default class App extends Component {
    render() {
        return (
                <Layout>
                    <Route exact path='/generate' component={Generate} />
                    <Route exact path='/upload' component={Upload} />
                    <Route exact path='/' component={Home} />
                </Layout>
        )
    }
}
