import React, { Component } from 'react'
import { Button, Container, Row, Col, Input } from 'reactstrap'
import '../styles/styles.css'

export default class NoteInput extends Component {
    constructor (props){
        super(props)

        this.state = { text: '' }
    }

    onClick = (e) => {
        this.props.create(this.state.text)
        this.setState({ text: '' })
        e.preventDefault()
    }

    onChange = (e) => {
        this.setState({ text: e.target.value });
    }

    render() {
        return (
            <Container className="note-input">
                <Row>
                    <Col>
                        <Input 
                            type="textarea" 
                            name="text" 
                            value={this.state.text} 
                            onChange={this.onChange} />
                    </Col>
                    <Col>
                        <Button 
                            outline 
                            onClick={this.onClick}>
                                Send...
                        </Button>
                    </Col>
                </Row>
            </Container>
        )
    }
}