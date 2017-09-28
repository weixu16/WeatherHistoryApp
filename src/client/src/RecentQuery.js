import React, { Component } from 'react';
import './App.css';
import 'whatwg-fetch';

class RecentQuery extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        
        return (
            <div>
                <div>Recent queried cities:</div>
                <li>Redmond</li>
                <li>Bellevue</li>
            </div>
        );
    }
}

export default RecentQuery;