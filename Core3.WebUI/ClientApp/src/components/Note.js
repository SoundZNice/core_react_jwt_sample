import React, { Component } from 'react'

export default class Note extends Component {
    constructor (props) {
        super(props);

        this.state = { 
            id: this.props.id, 
            text: this.props.text 
        };
    }

    render() {
        return (
            <div className="note">
                <span>{this.state.text}</span>
            </div>
        )
    }
}