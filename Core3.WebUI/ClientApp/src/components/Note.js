import React, { Component } from 'react'
import { Toast, ToastBody, ToastHeader } from 'reactstrap';
import '../styles/styles.css'

export default class Note extends Component {
    constructor (props) {
        super(props);

        this.state = { 
            id: this.props.id, 
            text: this.props.text,
            date: new Date(this.props.date).toLocaleString()
        };
    }

    render() {
        return (
            <Toast className="note">
                <ToastHeader>
                    <span className="note-date">{this.state.date}</span>
                </ToastHeader>
                <ToastBody>
                    <span>{this.state.text}</span>
                </ToastBody>
            </Toast>
        )
    }
}