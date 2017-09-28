import React, { Component } from 'react';
import './App.css';
import 'whatwg-fetch';
//import RecentQuery from './RecentQuery';

var LineChart = require("react-chartjs").Line;

class App extends Component {
  constructor(props) {
    super(props);
    this.state = { city: 'Seattle', high: [], low: [], labels: [], error: '' };
  }

  componentWillMount() {
      this.fetchWeatherInfo();
  }

  render() {
    var lineChart = <div />;
    if (this.state.labels.length > 0) {
      var chartData = {
        labels: this.state.labels,
        datasets: [
          {
            data: this.state.high,
            fillColor: "rgba(0,100,0,0.2)",
            label: "High temp",
            pointColor: "rgba(0,100,0,1)",
            pointHighlightFill: "#fff",
            pointHighlightStroke: "rgba(0,100,0,1)",
            pointStrokeColor: "#fff",
            strokeColor: "rgba(0,100,0,1)"
          },
          {
            data: this.state.low,
            fillColor: "rgba(151,187,205,0.2)",
            label: "Low temp",
            pointColor: "rgba(151,187,205,1)",
            pointHighlightFill: "#fff",
            pointHighlightStroke: "rgba(151,187,205,1)",
            pointStrokeColor: "#fff",
            strokeColor: "rgba(151,187,205,1)"
          }
        ]
      };
      var chartOptions = {};

      lineChart = <LineChart data={chartData} options={chartOptions} width="800" height="350" />;
    }

    return (
      <div className="App">
        <div className="App-header">
          <div className="middle">
          <div className="headerTitle">Weather History Query</div>
          <input className="input" type="text" onChange={this.onchange.bind(this)} name="city"/>
          <button className="query" type="button" onClick={this.onclick.bind(this)} >Query</button>
          </div>
        </div>
        <div className="error">{this.state.error}</div>
        <div className="mainDiv">
          <div className="title">Weather history of city {this.state.city}: </div>
          <div className="chart">
            {lineChart}
          </div>
        </div>
        <div className="author">
          Developed by <a href="mailto:michellewx16@gmail.com">Michelle Xu</a> 
        </div>
      </div>
    );
  }

  fetchWeatherInfo() {
    var url = "/api/weather/getHistory/";
    var endpoint = url + this.state.city;

    var component = this;
    fetch(endpoint)
      .then(function (response) {
        return response.json()
      }).then(function (json) {
        if (json) {
          component.setState({ high: json.HighTemp });
          component.setState({ low: json.LowTemp });
          component.setState({ labels: json.Date });
          if(json.HighTemp.length == 1) {
            component.setState({ error: "The city {0} is just added to the monitoring list.".replace('{0}', json.City) });
          }
          else {
            component.setState({ error: "" });
          }
        }
        else {
          component.setState({ error: "The city name is invalid!" });
        }

      }).catch(function (ex) {
        console.log('parsing failed', ex)
      })

    /*
    this.setState({ high: [65, 59, 80, 81, 56, 55, 40] });
    this.setState({ low: [59, 80, 81, 56, 55, 40, 10] });
    this.setState({ labels: ["Jan", "Feb", "Mar", "April", "May", "Jun", "July", "Aug"] });
    */
  }

  onclick(event) {
    this.fetchWeatherInfo();
  }

  onchange(event) {
    this.setState({ city: event.target.value })
  }
}

export default App;
